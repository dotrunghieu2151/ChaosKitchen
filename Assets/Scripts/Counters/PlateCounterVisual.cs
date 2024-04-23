using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform _counterTopPoint;
    [SerializeField] private Transform _plateVisualPrefab;
    [SerializeField] private PlateCounter _plateCounter;

    private List<GameObject> _plateVisualGameObjectList;
    private const float PLATE_OFFSET_Y = 0.1f;

    private void Awake()
    {
        _plateVisualGameObjectList = new List<GameObject>();
    }
    private void Start()
    {
        _plateCounter.OnPlateSpawned += (sender, args) =>
        {
            Transform plateVisualTransform = Instantiate(_plateVisualPrefab, _counterTopPoint);
            plateVisualTransform.localPosition = new Vector3(0, PLATE_OFFSET_Y * _plateVisualGameObjectList.Count, 0);

            _plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
        };

        _plateCounter.OnPlateRemoved += (sender, args) =>
        {
            GameObject plateGameObject = _plateVisualGameObjectList.Last();
            _plateVisualGameObjectList.RemoveAt(_plateVisualGameObjectList.Count - 1);
            Destroy(plateGameObject);
        };
    }
}
