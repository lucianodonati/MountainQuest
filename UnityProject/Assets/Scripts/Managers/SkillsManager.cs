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
    public void ToggleSphere(SpheresId _id, bool _state)
    {
        SetPrefabs temp = new SetPrefabs();
        temp.active = _state;
        temp.id = _id;
        temp.prefab = spheres[(int)_id].prefab;
        spheres[(int)_id] = temp;
    }

    //private void UpdateEnumerator()
    //{
    //    List<SetPrefabs>.Enumerator iter = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().iter;
    //    List<SetPrefabs>.Enumerator tempIter = iter;
    //    iter = spheres.GetEnumerator();
    //    iter.MoveNext();
    //    if (spheres.Contains(tempIter.Current))
    //        while (iter.Current.id != tempIter.Current.id && iter.MoveNext()) ;
    //}
}