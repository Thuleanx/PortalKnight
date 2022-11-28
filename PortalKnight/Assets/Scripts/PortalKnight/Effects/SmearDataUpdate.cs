using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Thuleanx.PortalKnight.Effects {
	[RequireComponent(typeof(Renderer))]
	public class SmearDataUpdate : MonoBehaviour {
		// public string PrevPositionPropertyName = "_PrevPosition";
		public string VelocityPropertyName = "_Velocity";

		public Renderer Rend {get; private set;}
		public Material Mat => Rend.material; // ehhhh not always true but ok
		public bool Write = false;

		[SerializeField, Tooltip("How much time does the smear lag behind"), Range(0,1f)]
		float smearLagTime = 1f;

		void Awake() {
			Rend = GetComponent<Renderer>();
		}

		Queue<Vector3> positions = new Queue<Vector3>();
		Queue<Vector3> displacements = new Queue<Vector3>();
		Queue<float> times = new Queue<float>();
		float ctime = 0;
		float totDisplacement;
		Vector3 lastPos;

		void LateUpdate() {
			Vector3 pos = transform.position;

			while (ctime > smearLagTime) {
				positions.Dequeue();
				totDisplacement -= displacements.Dequeue().magnitude;
				ctime -= times.Dequeue();
			}

			ctime += Time.deltaTime;
			totDisplacement += (pos - lastPos).magnitude;
			displacements.Enqueue(pos - lastPos);
			positions.Enqueue(pos);
			times.Enqueue(Time.deltaTime);


			if (Write) {
				if (Mat.HasProperty(VelocityPropertyName) && displacements.Count > 0)
					Mat?.SetVector(VelocityPropertyName, (pos - positions.Peek()) / ctime);
				else Mat?.SetVector(VelocityPropertyName, Vector3.zero);
			}

			lastPos = pos;
		}
	}
}