using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics;

public class TileBatch
{
	private struct SpriteData
	{
		public Vector4 Source;
		public Vector4 Destination;
		public Vector2 Origin;
		public SpriteEffects Effects;
		public VertexColors Colors;
		public float Rotation;
	}

	private static readonly float[] CORNER_OFFSET_X = new float[4] {
		0f,
		1f,
		1f,
		0f
	};
	private static readonly float[] CORNER_OFFSET_Y = new float[4] {
		0f,
		0f,
		1f,
		1f
	};
	private GraphicsDevice _graphicsDevice;
	private SpriteData[] _spriteDataQueue = new SpriteData[2048];
	private Texture2D[] _spriteTextures;
	private int _queuedSpriteCount;
	private SpriteBatch _spriteBatch;
	private static Vector2 _vector2Zero;
	private static Rectangle? _nullRectangle;
	private DynamicVertexBuffer _vertexBuffer;
	private DynamicIndexBuffer _indexBuffer;
	private short[] _fallbackIndexData;
	private VertexPositionColorTexture[] _vertices = new VertexPositionColorTexture[8192];
	private int _vertexBufferPosition;

	public TileBatch(GraphicsDevice graphicsDevice)
	{
		_graphicsDevice = graphicsDevice;
		_spriteBatch = new SpriteBatch(graphicsDevice);
		Allocate();
	}

	private void Allocate()
	{
		if (_vertexBuffer == null || _vertexBuffer.IsDisposed) {
			_vertexBuffer = new DynamicVertexBuffer(_graphicsDevice, typeof(VertexPositionColorTexture), 8192, BufferUsage.WriteOnly);
			_vertexBufferPosition = 0;
			_vertexBuffer.ContentLost += delegate {
				_vertexBufferPosition = 0;
			};
		}

		if (_indexBuffer != null && !_indexBuffer.IsDisposed)
			return;

		if (_fallbackIndexData == null) {
			_fallbackIndexData = new short[12288];
			for (int i = 0; i < 2048; i++) {
				_fallbackIndexData[i * 6] = (short)(i * 4);
				_fallbackIndexData[i * 6 + 1] = (short)(i * 4 + 1);
				_fallbackIndexData[i * 6 + 2] = (short)(i * 4 + 2);
				_fallbackIndexData[i * 6 + 3] = (short)(i * 4);
				_fallbackIndexData[i * 6 + 4] = (short)(i * 4 + 2);
				_fallbackIndexData[i * 6 + 5] = (short)(i * 4 + 3);
			}
		}

		_indexBuffer = new DynamicIndexBuffer(_graphicsDevice, typeof(short), 12288, BufferUsage.WriteOnly);
		_indexBuffer.SetData(_fallbackIndexData);
		_indexBuffer.ContentLost += delegate {
			_indexBuffer.SetData(_fallbackIndexData);
		};
	}

	private void FlushRenderState()
	{
		Allocate();
		_graphicsDevice.SetVertexBuffer(_vertexBuffer);
		_graphicsDevice.Indices = _indexBuffer;
		_graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
	}

	public void Dispose()
	{
		if (_vertexBuffer != null)
			_vertexBuffer.Dispose();

		if (_indexBuffer != null)
			_indexBuffer.Dispose();
	}

	public void Begin(RasterizerState rasterizer, Matrix transformation)
	{
		_spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, rasterizer, null, transformation);
		_spriteBatch.End();
	}

	public void Begin()
	{
		_spriteBatch.Begin();
		_spriteBatch.End();
	}

	public void Draw(Texture2D texture, Vector2 position, VertexColors colors)
	{
		Vector4 destination = default(Vector4);
		destination.X = position.X;
		destination.Y = position.Y;
		destination.Z = 1f;
		destination.W = 1f;
		InternalDraw(texture, ref destination, scaleDestination: true, ref _nullRectangle, ref colors, ref _vector2Zero, SpriteEffects.None, 0f);
	}

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors colors, Vector2 origin, float scale, SpriteEffects effects)
	{
		Vector4 destination = default(Vector4);
		destination.X = position.X;
		destination.Y = position.Y;
		destination.Z = scale;
		destination.W = scale;
		InternalDraw(texture, ref destination, scaleDestination: true, ref sourceRectangle, ref colors, ref origin, effects, 0f);
	}

	public void Draw(Texture2D texture, Vector4 destination, VertexColors colors)
	{
		InternalDraw(texture, ref destination, scaleDestination: false, ref _nullRectangle, ref colors, ref _vector2Zero, SpriteEffects.None, 0f);
	}

	public void Draw(Texture2D texture, Vector2 position, VertexColors colors, Vector2 scale)
	{
		Vector4 destination = default(Vector4);
		destination.X = position.X;
		destination.Y = position.Y;
		destination.Z = scale.X;
		destination.W = scale.Y;
		InternalDraw(texture, ref destination, scaleDestination: true, ref _nullRectangle, ref colors, ref _vector2Zero, SpriteEffects.None, 0f);
	}

	public void Draw(Texture2D texture, Vector4 destination, Rectangle? sourceRectangle, VertexColors colors)
	{
		InternalDraw(texture, ref destination, scaleDestination: false, ref sourceRectangle, ref colors, ref _vector2Zero, SpriteEffects.None, 0f);
	}

	public void Draw(Texture2D texture, Vector4 destination, Rectangle? sourceRectangle, VertexColors colors, Vector2 origin, SpriteEffects effects, float rotation)
	{
		InternalDraw(texture, ref destination, scaleDestination: false, ref sourceRectangle, ref colors, ref origin, effects, rotation);
	}

	public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, VertexColors colors)
	{
		Vector4 destination = default(Vector4);
		destination.X = destinationRectangle.X;
		destination.Y = destinationRectangle.Y;
		destination.Z = destinationRectangle.Width;
		destination.W = destinationRectangle.Height;
		InternalDraw(texture, ref destination, scaleDestination: false, ref sourceRectangle, ref colors, ref _vector2Zero, SpriteEffects.None, 0f);
	}

	private static short[] CreateIndexData()
	{
		short[] array = new short[12288];
		for (int i = 0; i < 2048; i++) {
			array[i * 6] = (short)(i * 4);
			array[i * 6 + 1] = (short)(i * 4 + 1);
			array[i * 6 + 2] = (short)(i * 4 + 2);
			array[i * 6 + 3] = (short)(i * 4);
			array[i * 6 + 4] = (short)(i * 4 + 2);
			array[i * 6 + 5] = (short)(i * 4 + 3);
		}

		return array;
	}

	private unsafe void InternalDraw(Texture2D texture, ref Vector4 destination, bool scaleDestination, ref Rectangle? sourceRectangle, ref VertexColors colors, ref Vector2 origin, SpriteEffects effects, float rotation)
	{
		if (_queuedSpriteCount >= _spriteDataQueue.Length)
			Array.Resize(ref _spriteDataQueue, _spriteDataQueue.Length << 1);

		fixed (SpriteData* ptr = &_spriteDataQueue[_queuedSpriteCount]) {
			float num = destination.Z;
			float num2 = destination.W;
			if (sourceRectangle.HasValue) {
				Rectangle value = sourceRectangle.Value;
				ptr->Source.X = value.X;
				ptr->Source.Y = value.Y;
				ptr->Source.Z = value.Width;
				ptr->Source.W = value.Height;
				if (scaleDestination) {
					num *= (float)value.Width;
					num2 *= (float)value.Height;
				}
			}
			else {
				float num3 = texture.Width;
				float num4 = texture.Height;
				ptr->Source.X = 0f;
				ptr->Source.Y = 0f;
				ptr->Source.Z = num3;
				ptr->Source.W = num4;
				if (scaleDestination) {
					num *= num3;
					num2 *= num4;
				}
			}

			ptr->Destination.X = destination.X;
			ptr->Destination.Y = destination.Y;
			ptr->Destination.Z = num;
			ptr->Destination.W = num2;
			ptr->Origin.X = origin.X;
			ptr->Origin.Y = origin.Y;
			ptr->Effects = effects;
			ptr->Colors = colors;
			ptr->Rotation = rotation;
		}

		if (_spriteTextures == null || _spriteTextures.Length != _spriteDataQueue.Length)
			Array.Resize(ref _spriteTextures, _spriteDataQueue.Length);

		_spriteTextures[_queuedSpriteCount++] = texture;
	}

	public void End()
	{
		if (_queuedSpriteCount != 0) {
			FlushRenderState();
			Flush();
		}
	}

	private void Flush()
	{
		Texture2D texture2D = null;
		int num = 0;
		for (int i = 0; i < _queuedSpriteCount; i++) {
			if (_spriteTextures[i] != texture2D) {
				if (i > num)
					RenderBatch(texture2D, _spriteDataQueue, num, i - num);

				num = i;
				texture2D = _spriteTextures[i];
			}
		}

		RenderBatch(texture2D, _spriteDataQueue, num, _queuedSpriteCount - num);
		Array.Clear(_spriteTextures, 0, _queuedSpriteCount);
		_queuedSpriteCount = 0;
	}

	private unsafe void RenderBatch(Texture2D texture, SpriteData[] sprites, int offset, int count)
	{
		_graphicsDevice.Textures[0] = texture;
		float num = 1f / (float)texture.Width;
		float num2 = 1f / (float)texture.Height;
		while (count > 0) {
			SetDataOptions options = SetDataOptions.NoOverwrite;
			int num3 = count;
			if (num3 > 2048 - _vertexBufferPosition) {
				num3 = 2048 - _vertexBufferPosition;
				if (num3 < 256) {
					_vertexBufferPosition = 0;
					options = SetDataOptions.Discard;
					num3 = count;
					if (num3 > 2048)
						num3 = 2048;
				}
			}

			fixed (SpriteData* ptr = &sprites[offset]) {
				fixed (VertexPositionColorTexture* ptr3 = &_vertices[0]) {
					SpriteData* ptr2 = ptr;
					VertexPositionColorTexture* ptr4 = ptr3;
					for (int i = 0; i < num3; i++) {
						float num4;
						float num5;
						if (ptr2->Rotation != 0f) {
							num4 = (float)Math.Cos(ptr2->Rotation);
							num5 = (float)Math.Sin(ptr2->Rotation);
						}
						else {
							num4 = 1f;
							num5 = 0f;
						}

						float num6 = ptr2->Origin.X / ptr2->Source.Z;
						float num7 = ptr2->Origin.Y / ptr2->Source.W;
						ptr4->Color = ptr2->Colors.TopLeftColor;
						ptr4[1].Color = ptr2->Colors.TopRightColor;
						ptr4[2].Color = ptr2->Colors.BottomRightColor;
						ptr4[3].Color = ptr2->Colors.BottomLeftColor;
						for (int j = 0; j < 4; j++) {
							float num8 = CORNER_OFFSET_X[j];
							float num9 = CORNER_OFFSET_Y[j];
							float num10 = (num8 - num6) * ptr2->Destination.Z;
							float num11 = (num9 - num7) * ptr2->Destination.W;
							float x = ptr2->Destination.X + num10 * num4 - num11 * num5;
							float y = ptr2->Destination.Y + num10 * num5 + num11 * num4;
							if ((ptr2->Effects & SpriteEffects.FlipVertically) != 0)
								num9 = 1f - num9;

							if ((ptr2->Effects & SpriteEffects.FlipHorizontally) != 0)
								num8 = 1f - num8;

							ptr4->Position.X = x;
							ptr4->Position.Y = y;
							ptr4->Position.Z = 0f;
							ptr4->TextureCoordinate.X = (ptr2->Source.X + num8 * ptr2->Source.Z) * num;
							ptr4->TextureCoordinate.Y = (ptr2->Source.Y + num9 * ptr2->Source.W) * num2;
							ptr4++;
						}

						ptr2++;
					}
				}
			}

			int offsetInBytes = _vertexBufferPosition * System.Runtime.CompilerServices.Unsafe.SizeOf<VertexPositionColorTexture>() * 4;
			_vertexBuffer.SetData(offsetInBytes, _vertices, 0, num3 * 4, System.Runtime.CompilerServices.Unsafe.SizeOf<VertexPositionColorTexture>(), options);
			int minVertexIndex = _vertexBufferPosition * 4;
			int numVertices = num3 * 4;
			int startIndex = _vertexBufferPosition * 6;
			int primitiveCount = num3 * 2;
			_graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, minVertexIndex, numVertices, startIndex, primitiveCount);
			_vertexBufferPosition += num3;
			offset += num3;
			count -= num3;
		}
	}
}
