using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TemplatesDictionaryConfig", menuName = "TemplatesDictionary", order = 0)]
public class TemplatesDictionary : ScriptableObject
{
    [SerializeField] private List<GameObject> boatPreview = new List<GameObject>();
    [SerializeField] private List<GameObject> boatTemplate = new List<GameObject>();
    [SerializeField] private List<GameObject> cannonPreview = new List<GameObject>();
    [SerializeField] private List<GameObject> cannonTemplate = new List<GameObject>();

    public List<GameObject> BoatPreview => boatPreview;
    public List<GameObject> BoatTemplate => boatTemplate;
    public List<GameObject> CannonPreview => cannonPreview;
    public List<GameObject> CannonTemplate => cannonTemplate;

}
