using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MonkeyScript : MonoBehaviour {

    public float timeToHide = 1f;
    private Image _image;

    private bool _isAlphaDecrementing = false;
    // Start is called before the first frame update


    void Start()
    {
        _image = GetComponent<Image>();
        Invoke("HidePanel", timeToHide);
        
    }

    private void HidePanel()
    {
        _isAlphaDecrementing = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isAlphaDecrementing)
        {
            Color oldColor = _image.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a - 0.01f);
            _image.color = newColor;
            if(newColor.a == 0)
            {
                _isAlphaDecrementing = false;
                gameObject.SetActive(false);
            }
        }
    }
}
