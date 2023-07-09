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

    public void AddBoatPreview(GameObject boat)
    {
        boatPreview.Add(boat);
    }
    
    public List<GameObject> BoatTemplate => boatTemplate;
    
    public void AddBoatTemplate(GameObject boat)
    {
        boatTemplate.Add(boat);
    }
    
    public List<GameObject> CannonPreview => cannonPreview;
    
    public void AddCannonPreview(GameObject cannon)
    {
        cannonPreview.Add(cannon);
    }
    
    public List<GameObject> CannonTemplate => cannonTemplate;

    public void AddCannonTemplate(GameObject cannon)
    {
        cannonTemplate.Add(cannon);
    }
}
