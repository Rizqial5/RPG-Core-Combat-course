using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour 
    {
        const float waypointsRadius = 0.3f;
        private void OnDrawGizmos() {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = NextWaypointsIndex(i);
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(GetWaypoints(i), waypointsRadius);
                Gizmos.DrawLine(GetWaypoints(i), GetWaypoints(j));


            }
        }

        public  int NextWaypointsIndex(int i)
        {
            if(i+1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        public Vector3 GetWaypoints(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}