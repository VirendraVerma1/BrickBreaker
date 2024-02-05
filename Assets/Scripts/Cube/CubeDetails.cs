using TMPro;
using UnityEngine;

public class CubeDetails : MonoBehaviour
{
    public TextMeshProUGUI myCountText;
    public int count = 10;
    public GameObject destroyedObject;
    private void Start()
    {
        myCountText.text = count.ToString();
        destroyedObject.SetActive(false);
    }

    public void DecreaseNumber()
    {
        count--;
        myCountText.text = count.ToString();
        if (count <= 0)
        {
            destroyedObject.SetActive(true);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            myCountText.text = "";
            //Destroy(gameObject,10);
        }
    }
}
