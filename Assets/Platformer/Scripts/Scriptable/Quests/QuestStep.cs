using System;

namespace Platformer.Scriptable.Quests
{
	public enum QuestStepType : byte
	{
		FindItem = 0,
	}

	[Serializable]
	public struct QuestStep
	{
		public QuestStepType StepType;
		public string Data;
	}
}