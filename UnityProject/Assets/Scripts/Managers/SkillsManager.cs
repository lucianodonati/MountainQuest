using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    public List<SetPrefabs> spheres;

    [System.Serializable]
    public struct SetPrefabs
    {
        public SpheresId id;
        public bool active;
        public GameObject prefab;
    }

    [System.Serializable]
    public enum SpheresId
    {
        Boost, Shield, Wave, Redirect
    }

    public void Awake()
    {
        transform.parent = GameManager.instance.transform;
    }

    // Use this for initialization
    public void SetSphere(SpheresId _id, bool _state)
    {
        SetPrefabs temp = new SetPrefabs();
        temp.active = _state;
        temp.id = _id;
        temp.prefab = spheres[(int)_id].prefab;
        spheres[(int)_id] = temp;
    }

    public bool CheckEmpty()
    {
        bool isEmpty = true;
        foreach (SetPrefabs item in spheres)
        {
            if (item.active)
            {
                isEmpty = false;
                break;
            }
        }
        return isEmpty;
    }
}