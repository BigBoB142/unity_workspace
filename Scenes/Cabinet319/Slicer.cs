using UnityEngine;
using EzySlice;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Slicer : MonoBehaviour
{
    [Tooltip("Материал, который будет внутри разреза")]
    public Material insideMaterial;

    [Tooltip("Слои объектов, которые можно резать")]
    public LayerMask sliceableLayer;

    [Tooltip("Активирует одиночный разрез при следующем кадре")]
    public bool trigger = false;

    void Update()
    {
        if (trigger)
        {
            // Исправлено: в Physics.Raycast передается НАПРАВЛЕНИЕ (transform.forward)
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 2.2f, sliceableLayer))
            {
                // ИСПРАВЛЕНИЕ 1: Нормаль плоскости теперь берется от направления "вверх" вашего объекта.
                // Если вы наклоните объект-нож вбок, то и плоскость разреза наклонится соответственно!
                Vector3 cutNormal = transform.forward;

                SliceObject(hit.collider.gameObject, hit.point, cutNormal);
            }
        }
    }

    public void SliceObject(GameObject obj, Vector3 planePosition, Vector3 planeNormal)
    {
        SlicedHull hull = obj.Slice(planePosition, planeNormal, insideMaterial);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(obj, insideMaterial);
            GameObject lowerHull = hull.CreateLowerHull(obj, insideMaterial);

            // Рекомендуется вернуть физику, чтобы половинки сдвинулись с места и разрез стал заметен
            SetupPhysics(upperHull);
            SetupPhysics(lowerHull);

            Destroy(obj);
        }
    }

    private void SetupPhysics(GameObject obj)
    {
        MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
        meshCollider.convex = true;

        obj.layer = 3;
        obj.AddComponent<XRGrabInteractable>();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        // Небольшой импульс в стороны, чтобы половинки красиво разошлись
        rb.AddExplosionForce(50f, obj.transform.position, 1f);
    }

    public void StartTrigger()
    {
        trigger = true;
    }

    public void StopTrigger()
    {
        // В текущей логике одиночного удара этот метод больше не обязателен, 
        // так как триггер сам тухнет в Update
        trigger = false;
    }

    private void OnDrawGizmos()
    {   
        Gizmos.color = Color.red;
        // ИСПРАВЛЕНИЕ 3: Теперь гизмо рисует линию из позиции объекта в точку на 10 метров впереди него
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 2.2f));
    }
}
