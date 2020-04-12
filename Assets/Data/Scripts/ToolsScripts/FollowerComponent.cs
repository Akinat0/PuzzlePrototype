using UnityEngine;

public class FollowerComponent : MonoBehaviour
{
   [SerializeField] public Transform Target;

   private void LateUpdate()
   {
      if (Target != null)
      {
         transform.position = Target.transform.position;
         transform.rotation = Target.transform.rotation;
      }
   }
}
