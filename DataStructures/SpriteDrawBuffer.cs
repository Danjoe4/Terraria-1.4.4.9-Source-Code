using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;

namespace Terraria.DataStructures;

public class SpriteDrawBuffer
{
	private GraphicsDevice graphicsDevice;
	private DynamicVertexBuffer vertexBuffer;
	private IndexBuffer indexBuffer;
	private VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[0];
	private Texture[] textures = new Texture[0];
	private int maxSprites;
	private int vertexCount;
	private VertexBufferBinding[] preBindVertexBuffers;
	private IndexBuffer preBindIndexBuffer;

	public SpriteDrawBuffer(GraphicsDevice graphicsDevice, int defaultSize)
	{
		this.graphicsDevice = graphicsDevice;
		maxSprites = defaultSize;
		CreateBuffers();
	}

	public void CheckGraphicsDevice(GraphicsDevice graphicsDevice)
	{
		if (this.graphicsDevice != graphicsDevice) {
			this.graphicsDevice = graphicsDevice;
			CreateBuffers();
		}
	}

	private void CreateBuffers()
	{
		if (vertexBuffer != null)
			vertexBuffer.Dispose();

		vertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), maxSprites * 4, BufferUsage.WriteOnly);
		if (indexBuffer != null)
			indexBuffer.Dispose();

		indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), maxSprites * 6, BufferUsage.WriteOnly);
		indexBuffer.SetData(GenIndexBuffer(maxSprites));
		Array.Resize(ref vertices, maxSprites * 6);
		Array.Resize(ref textures, maxSprites);
	}

	private static ushort[] GenIndexBuffer(int maxSprites)
	{
		ushort[] array = new ushort[maxSprites * 6];
		int num = 0;
		ushort num2 = 0;
		while (num < maxSprites) {
			array[num++] = num2;
			array[num++] = (ushort)(num2 + 1);
			array[num++] = (ushort)(num2 + 2);
			array[num++] = (ushort)(num2 + 3);
			array[num++] = (ushort)(num2 + 2);
			array[num++] = (ushort)(num2 + 1);
			num2 = (ushort)(num2 + 4);
		}

		return array;
	}

	public void UploadAndBind()
	{
		if (vertexCount > 0)
			vertexBuffer.SetData(vertices, 0, vertexCount, SetDataOptions.Discard);

		vertexCount = 0;
		Bind();
	}

	public void Bind()
	{
		preBindVertexBuffers = graphicsDevice.GetVertexBuffers();
		preBindIndexBuffer = graphicsDevice.Indices;
		graphicsDevice.SetVertexBuffer(vertexBuffer);
		graphicsDevice.Indices = indexBuffer;
	}

	public void Unbind()
	{
		graphicsDevice.SetVertexBuffers(preBindVertexBuffers);
		graphicsDevice.Indices = preBindIndexBuffer;
		preBindVertexBuffers = null;
		preBindIndexBuffer = null;
	}

	public void DrawRange(int index, int count)
	{
		graphicsDevice.Textures[0] = textures[index];
		graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, index * 4, 0, count * 4, 0, count * 2);
	}

	public void DrawSingle(int index)
	{
		DrawRange(index, 1);
	}

	public void Draw(Texture2D texture, Vector2 position, VertexColors colors)
	{
		Draw(texture, position, null, colors, 0f, Vector2.Zero, 1f, SpriteEffects.None);
	}

	public void Draw(Texture2D texture, Rectangle destination, VertexColors colors)
	{
		Draw(texture, destination, null, colors);
	}

	public void Draw(Texture2D texture, Rectangle destination, Rectangle? sourceRectangle, VertexColors colors)
	{
		Draw(texture, destination, sourceRectangle, colors, 0f, Vector2.Zero, SpriteEffects.None);
	}

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors color, float rotation, Vector2 origin, float scale, SpriteEffects effects)
	{
		Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale, scale), effects);
	}

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
	{
		float z;
		float w;
		if (sourceRectangle.HasValue) {
			z = (float)sourceRectangle.Value.Width * scale.X;
			w = (float)sourceRectangle.Value.Height * scale.Y;
		}
		else {
			z = (float)texture.Width * scale.X;
			w = (float)texture.Height * scale.Y;
		}

		Draw(texture, new Vector4(position.X, position.Y, z, w), sourceRectangle, colors, rotation, origin, effects, 0f);
	}

	public void Draw(Texture2D texture, Rectangle destination, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, SpriteEffects effects)
	{
		Draw(texture, new Vector4(destination.X, destination.Y, destination.Width, destination.Height), sourceRectangle, colors, rotation, origin, effects, 0f);
	}

	public void Draw(Texture2D texture, Vector4 destinationRectangle, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, SpriteEffects effect, float depth)
	{
		Vector4 sourceRectangle2 = default(Vector4);
		if (sourceRectangle.HasValue) {
			sourceRectangle2.X = sourceRectangle.Value.X;
			sourceRectangle2.Y = sourceRectangle.Value.Y;
			sourceRectangle2.Z = sourceRectangle.Value.Width;
			sourceRectangle2.W = sourceRectangle.Value.Height;
		}
		else {
			sourceRectangle2.X = 0f;
			sourceRectangle2.Y = 0f;
			sourceRectangle2.Z = texture.Width;
			sourceRectangle2.W = texture.Height;
		}

		Vector2 texCoordTL = default(Vector2);
		texCoordTL.X = sourceRectangle2.X / (float)texture.Width;
		texCoordTL.Y = sourceRectangle2.Y / (float)texture.Height;
		Vector2 texCoordBR = default(Vector2);
		texCoordBR.X = (sourceRectangle2.X + sourceRectangle2.Z) / (float)texture.Width;
		texCoordBR.Y = (sourceRectangle2.Y + sourceRectangle2.W) / (float)texture.Height;
		if ((effect & SpriteEffects.FlipVertically) != 0) {
			float y = texCoordBR.Y;
			texCoordBR.Y = texCoordTL.Y;
			texCoordTL.Y = y;
		}

		if ((effect & SpriteEffects.FlipHorizontally) != 0) {
			float x = texCoordBR.X;
			texCoordBR.X = texCoordTL.X;
			texCoordTL.X = x;
		}

		QueueSprite(destinationRectangle, -origin, colors, sourceRectangle2, texCoordTL, texCoordBR, texture, depth, rotation);
	}

	private void QueueSprite(Vector4 destinationRect, Vector2 origin, VertexColors colors, Vector4 sourceRectangle, Vector2 texCoordTL, Vector2 texCoordBR, Texture2D texture, float depth, float rotation)
	{
		float num = origin.X / sourceRectangle.Z;
		float num2 = origin.Y / sourceRectangle.W;
		float x = destinationRect.X;
		float y = destinationRect.Y;
		float z = destinationRect.Z;
		float w = destinationRect.W;
		float num3 = num * z;
		float num4 = num2 * w;
		float num5;
		float num6;
		if (rotation != 0f) {
			num5 = (float)Math.Cos(rotation);
			num6 = (float)Math.Sin(rotation);
		}
		else {
			num5 = 1f;
			num6 = 0f;
		}

		if (vertexCount + 4 >= maxSprites * 4) {
			maxSprites *= 2;
			CreateBuffers();
		}

		textures[vertexCount / 4] = texture;
		PushVertex(new Vector3(x + num3 * num5 - num4 * num6, y + num3 * num6 + num4 * num5, depth), colors.TopLeftColor, texCoordTL);
		PushVertex(new Vector3(x + (num3 + z) * num5 - num4 * num6, y + (num3 + z) * num6 + num4 * num5, depth), colors.TopRightColor, new Vector2(texCoordBR.X, texCoordTL.Y));
		PushVertex(new Vector3(x + num3 * num5 - (num4 + w) * num6, y + num3 * num6 + (num4 + w) * num5, depth), colors.BottomLeftColor, new Vector2(texCoordTL.X, texCoordBR.Y));
		PushVertex(new Vector3(x + (num3 + z) * num5 - (num4 + w) * num6, y + (num3 + z) * num6 + (num4 + w) * num5, depth), colors.BottomRightColor, texCoordBR);
	}

	private void PushVertex(Vector3 pos, Color color, Vector2 texCoord)
	{
		SetVertex(ref vertices[vertexCount++], pos, color, texCoord);
	}

	private static void SetVertex(ref VertexPositionColorTexture vertex, Vector3 pos, Color color, Vector2 texCoord)
	{
		vertex.Position = pos;
		vertex.Color = color;
		vertex.TextureCoordinate = texCoord;
	}
}
