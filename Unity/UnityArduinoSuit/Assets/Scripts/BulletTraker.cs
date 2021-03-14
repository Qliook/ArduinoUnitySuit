using System;
using UnityEngine;

public class BulletTraker : MonoBehaviour
{
    public Camera _camera;
    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            if (_camera == null) Debug.LogWarning("На сцене отсутствует камера!");
        }
    }

    public GameObject[] Points;
    public GameObject HitBullet;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //проверка на попадание в игрока
                if (hit.collider.tag != "Player") return;

                //Нахождение точки попадания
                FindNearestObjectToPoint(Points, hit.point, out GameObject nObject);
                //Debug.Log($"Название объекта: {nObject.name} \nНомер объекта в массиве: {Array.IndexOf(Points, nObject)}");

                // Отсылаем данные в виде номера объекта + e для завершения команды
                Arduino.SentToArduino($"{Array.IndexOf(Points, nObject)}e");

                #region Обозначение попадания и изменение цвета у близжайшей точки 

                GameObject hitBullet = Instantiate(HitBullet, hit.point, default);
                hitBullet.GetComponent<MeshRenderer>().material.color = Color.yellow;
                Destroy(hitBullet, 1);

                foreach (var item in Points)
                {
                    item.GetComponent<Renderer>().material.color = Color.white;
                }
                nObject.GetComponent<Renderer>().material.color = Color.red;

                #endregion
            }
        }
    }

    /// <summary>
    /// Нахождение близжайщего объекта к выбранной позиции
    /// </summary>
    /// <param name="Objects">Массив объектов для перебора</param>
    /// <param name="point">выбранная позиция</param>
    /// <param name="nearObj">близжайший объект</param>
    public static void FindNearestObjectToPoint(GameObject[] Objects, Vector3 point, out GameObject nearObj)
    {
        float distance = float.MaxValue;

        nearObj = Objects[0];

        foreach (var obj in Objects)
        {
            float distanceNow = Vector3.Distance(point, obj.transform.position);

            if (distanceNow < distance)
            {
                distance = distanceNow;
                nearObj = obj;
            }
        }
    }
    public static void FindNearestObjectToPoint(GameObject[] Objects, GameObject point, out GameObject nearObj)
    {
        FindNearestObjectToPoint(Objects, point.transform.position, out nearObj);
    }

    #region Variant 2 (Массив объектов = сами объекта попадания)

    //public GameObject[] Objects;

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

    //        if (Physics.Raycast(ray, out RaycastHit hit))
    //        {
    //            // проверка на попадание в игрока
    //            // тег необходимо вешать на объекты попадания
    //            if (hit.collider.tag != "Player") return;

    //            GameObject nObject = hit.collider.gameObject;

    //            // Отсылаем данные в виде номера объекта + e для завершения команды
    //            Arduino.SentToArduino($"{Array.IndexOf(Objects, nObject)}e");

    //            //Debug.Log($"Название объекта: {nObject.name} \nНомер объекта в массиве: {Array.IndexOf(Objects, nObject)}");
    //        }
    //    }
    //}

    #endregion

    #region Variant 3 (HitBullet = объект(не луч))

    //// Скрипт вешаеться на BulletPrefab или на Bullet

    //public GameObject[] Points;

    //private void OnTriggerEnter(Collider other)
    //{
    //    //проверка на попадание в игрока
    //    if (other.tag != "Player") return;

    //    Vector3 point = transform.position; //Сохраняем позицию пули в момент попадания по объекту

    //    BulletTraker.FindNearestObjectToPoint(Points, point, out GameObject nObject);

    //    //Отсылаем данные в виде номера объекта +e для завершения команды
    //    Arduino.SentToArduino($"{Array.IndexOf(Points, nObject)}e");

    //    //Debug.Log($"Название объекта: {nObject.name} \nНомер объекта в массиве: {Array.IndexOf(Points, nObject)}");
    //}

    #endregion
}
