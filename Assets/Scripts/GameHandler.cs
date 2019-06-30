using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
		var methodBase = MethodBase.GetCurrentMethod();
        Debug.Log($"{methodBase.DeclaringType}.{methodBase.Name}");
    }

    // Update is called once per frame
	private void Update()
    {
        
    }
}
