using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ChainSawSlicer : MonoBehaviour
{
    [Header("Точки для определения линии реза")]
    public Transform sliceStart;
    public Transform sliceEnd;

    [Header("Настройки резки")]
    [Tooltip("Материал для поверхности среза")]
    public Material crossSectionMaterial;
    [Tooltip("Задержка между резами в секундах")]
    public float sliceCooldown = 0.25f;

    [Header("Физика разрезанных частей")]
    public float explosionForce = 1.5f;

    [Header("Ориентация плоскости реза")]
    [Tooltip("Локальная ось пилы, задающая нормаль среза (обычно up или forward)")]
    public Vector3 cutNormalLocal = Vector3.forward;

    private bool isActive = false;
    private float lastSliceTime = -999f;
    private readonly HashSet<GameObject> slicedThisActivation = new HashSet<GameObject>();

    public void SawOn()
    {
        isActive = true;
        slicedThisActivation.Clear();
        // Сбрасываем таймер, чтобы первый рез после включения сработал сразу
        lastSliceTime = -999f;
    }

    public void SawOff()
    {
        isActive = false;
        slicedThisActivation.Clear();
    }

    public void OnReleased()
    {
        isActive = false;
        slicedThisActivation.Clear();
    }

    void OnTriggerStay(Collider other)
    {
        if (!isActive) return;
        if (other.isTrigger) return;
        if (other.tag != "sliceable") return;
        if (slicedThisActivation.Contains(other.gameObject)) return;

        // Кулдаун — защита от мгновенного повторного реза
        if (Time.time - lastSliceTime < sliceCooldown) return;

        Rigidbody targetRb = other.attachedRigidbody;
        if (targetRb == null) return;

        // 1. Направление лезвия
        Vector3 bladeDir = (sliceEnd.position - sliceStart.position).normalized;
        float bladeLength = Vector3.Distance(sliceStart.position, sliceEnd.position);

        // 2. Точка реза — проекция центра масс объекта на линию лезвия
        Vector3 toCenter = targetRb.worldCenterOfMass - sliceStart.position;
        float projection = Mathf.Clamp(Vector3.Dot(toCenter, bladeDir), 0f, bladeLength);
        Vector3 planePosition = sliceStart.position + bladeDir * projection;

        // 3. Нормаль плоскости реза из локальной оси пилы
        Vector3 planeNormal = transform.TransformDirection(cutNormalLocal).normalized;

        // 4. Режем
        SliceObject(targetRb.gameObject, planePosition, planeNormal, targetRb.velocity);

        // Обновляем время последнего реза
        lastSliceTime = Time.time;

        slicedThisActivation.Add(other.gameObject);
    }

    void SliceObject(GameObject target, Vector3 planePosition, Vector3 planeNormal, Vector3 originalVelocity)
    {
        SlicedHull hull = target.Slice(planePosition, planeNormal, crossSectionMaterial);
        if (hull == null) return;

        GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
        GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);

        SetupSlicedPart(upperHull, originalVelocity, planePosition);
        SetupSlicedPart(lowerHull, originalVelocity, planePosition);

        Destroy(target);
    }

    void SetupSlicedPart(GameObject part, Vector3 originalVelocity, Vector3 sliceCenter)
    {
        Rigidbody rb = part.AddComponent<Rigidbody>();
        MeshCollider collider = part.AddComponent<MeshCollider>();
        collider.convex = true;
        part.tag = "sliceable";
        part.AddComponent<XRGrabInteractable>();

        rb.velocity = originalVelocity;

        Vector3 pushDir = (part.transform.position - sliceCenter).normalized;
        rb.AddForce(pushDir * explosionForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * explosionForce, ForceMode.Impulse);
    }
}