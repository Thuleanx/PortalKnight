using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

using Thuleanx.AI.FSM;
using Thuleanx.Combat3D;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public partial class SpawnerEnemy {

		public enum State {
			Neutral,
			Spell,
			Dead
		}
	}

	public partial class SpawnerEnemy : Movable {
	}
}