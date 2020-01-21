using System.Collections;
using UnityEngine;

public class TagActionExample : MonoBehaviour
{
	[SerializeField] Camera worldCam;
	[HeaderAttribute("For Example01")]
	[SerializeField] Transform example01ParticleTrns;
	[SerializeField] AudioClip[] clips;
	[HeaderAttribute("For Example02")]
	[SerializeField] Animator animator;
	[SerializeField] GameObject example02ParticlePrefab;
	[HeaderAttribute("For Example03")]
	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] RectTransform imageTrns;
	RectTransform canvasGroupRectTrns;
	readonly int appearHash = Animator.StringToHash("Appear");
	readonly int bottleHash = Animator.StringToHash("Bottle");
	readonly int cakeHash = Animator.StringToHash("Cake");
	readonly int hornHash = Animator.StringToHash("Horn");
	AudioSource audioSource;
	ParticleSystem example01Particle;
	Vector2 baseSize;
	Vector2 startSize;
	Vector2 tempVec2;
	GameObject[] particlePool = new GameObject[6];

	Coroutine cortn;
	float example03TimeValue;
	bool isAppear03;

	void Awake()
	{
		canvasGroup.alpha = 0f;
		baseSize = imageTrns.sizeDelta;
		startSize = new Vector2(0f, baseSize.y * 2f);
		audioSource = GetComponent<AudioSource>();
		canvasGroupRectTrns = canvasGroup.GetComponent<RectTransform>();
		example01Particle = example01ParticleTrns.GetComponent<ParticleSystem>();
		Transform poolParentTrns = new GameObject("ParticlePool").transform;
		for(int i = 0; i < 6; i++)
		{
			particlePool[i] = Instantiate(example02ParticlePrefab, poolParentTrns);
			particlePool[i].SetActive(false);
		}
	}

	public void ExampleEvent01(string str, Vector2 screenPoint)
	{
		Debug.Log("ExampleEvent01 get string Argument: " + str);

		AudioClip pickClip = null;
		switch(str)
		{
			case "colored words":
				pickClip = clips[0];
				break;
			case "set to use":
				pickClip = clips[1];
				break;
			default:
				pickClip = clips[2];
				break;
		}
		if(audioSource.isPlaying)
		{
			audioSource.Stop();
			example01Particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
			if(audioSource.clip == pickClip)
				return;
		}
		audioSource.clip = pickClip;
		audioSource.Play();
		example01ParticleTrns.position = worldCam.ScreenToWorldPoint(screenPoint) + Vector3.forward;
		example01Particle.Play();
	}


	public void ExampleEvent02(string str, Vector2 screenPoint)
	{
		Debug.Log("ExampleEvent02 get string Argument: " + str);

		if(cortn != null)
		{
			StopCoroutine(cortn);
			cortn = null;
		}
		animator.SetBool(appearHash, false);
		switch(str)
		{
			case "ex_cake":
				animator.SetBool(cakeHash, true);
				break;
			case "ex_horn":
				animator.SetBool(hornHash, true);
				break;
			default:
				animator.SetBool(bottleHash, true);
				break;
		}
		cortn = StartCoroutine(Example02Coroutine());
	}

	IEnumerator Example02Coroutine()
	{
		yield return null;
		animator.SetBool(appearHash, true);
		yield return new WaitForSeconds(1.2f);
		int tempNum = Random.Range(3, 6);
		Vector3 particlePos = Vector3.zero;
		for(int i = 0; i < tempNum; i++)
		{
			if(particlePool[i].activeSelf)
				particlePool[i].SetActive(false);
			particlePos.Set(Random.Range(Screen.width * 0.2f, Screen.width * 0.8f), Random.Range(Screen.height * 0.2f, Screen.height * 0.8f), 1f);
			particlePool[i].transform.position = worldCam.ScreenToWorldPoint(particlePos);
			particlePool[i].SetActive(true);
			yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
		}
		yield break;
	}


	public void ExampleEvent03(string str, Vector2 screenPoint)
	{
		Debug.Log("ExampleEvent03 get string Argument: " + str);

		if(isAppear03)
			return;
		example03TimeValue = 0f;
		isAppear03 = true;
		canvasGroupRectTrns.position = screenPoint;
		tempVec2.Set(0f, baseSize.y * 0.5f);
		imageTrns.sizeDelta = tempVec2;
	}

	void Update()
	{
		if(isAppear03)
		{
			example03TimeValue += Time.deltaTime * 3f;
			if(example03TimeValue < 1f)
			{
				canvasGroup.alpha = example03TimeValue;
				imageTrns.sizeDelta = Vector2.Lerp(startSize, baseSize, example03TimeValue * 1.5f);
			}
			else
			{
				isAppear03 = false;
				canvasGroup.blocksRaycasts = true;
				canvasGroup.alpha = 1f;
				imageTrns.sizeDelta = baseSize;
			}
		}
	}

	public void OnCloseExample03()
	{
		isAppear03 = false;
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = false;
	}

}

