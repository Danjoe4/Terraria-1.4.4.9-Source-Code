namespace Terraria.Physics;

public struct BallStepResult
{
	public readonly BallState State;

	private BallStepResult(BallState state)
	{
		State = state;
	}

	public static BallStepResult OutOfBounds() => new BallStepResult(BallState.OutOfBounds);
	public static BallStepResult Moving() => new BallStepResult(BallState.Moving);
	public static BallStepResult Resting() => new BallStepResult(BallState.Resting);
}
