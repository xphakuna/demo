using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;



public partial class TestSafeCoroutine : MonoBehaviour
{


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{


		btnTryFinalize.onClick.AddListener(() =>
		{
			DoTryFinalize();
		});
	}

	// Update is called once per frame
	void Update()
	{

	}
}

