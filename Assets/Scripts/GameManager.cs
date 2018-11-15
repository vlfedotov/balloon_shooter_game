using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	[SerializeField] private GameObject _balloonSpawnerPrefab;
	
	[SerializeField] private Text _scoreText;
	[SerializeField] private Text _h1Text;
	[SerializeField] private Text _h2Text;
	[SerializeField] private Text _timeToFinishText;
	
	[SerializeField] private float _secondsToStartGame = 3f;
	[SerializeField] private float _secondsToGameOver = 120f;
	[SerializeField] private float _spawnSpeed = 0.6f;
	
	[SerializeField] private float _minimumBalloonSize = 0.2f;
	[SerializeField] private float _maximumBalloonSize = 5f;
	[SerializeField] private float _minimumBaseBalloonSpeed = 3f;
	[SerializeField] private float _maximumBaseBalloonSpeed = 10f;
	[SerializeField] private float _minimumBalloonSizeSpeed = 5f;
	[SerializeField] private float _maximumBalloonSizeSpeed = 15f;

	private int _remainingTimeInSeconds;
	private float _remainingTime;
	private int _score;
	
	void Start () {
		StartCoroutine(GameLoop());
		_h1Text.text = "Balloon Shooter";
	}

	IEnumerator GameLoop() {
		yield return StartCoroutine(WaitForKeyDown(KeyCode.Space));
		
		Balloon.OnClick += Score;
		float startTime = Time.time;
		do {
			_remainingTime = _secondsToStartGame - (Time.time - startTime);
			_h1Text.text = "Приготовьтесь";
			_h2Text.text = _remainingTime.ToString("0.00");
			yield return null;
		} while (_remainingTime > 0f);

		_h1Text.text = "Начинаем!";
		_h2Text.text = "0.00";
		yield return new WaitForSeconds(1f);

		_h1Text.transform.parent.gameObject.SetActive(false);
		StartCoroutine(SpawnBalloons());
	}
	
	IEnumerator SpawnBalloons() {
		_score = 0;
		_scoreText.text = "Очки: " + _score;
		_timeToFinishText.transform.parent.gameObject.SetActive(true);
		
		var balloonSpawnerObject = Instantiate(_balloonSpawnerPrefab, new Vector3(0f, 0f, -1f), Quaternion.identity);
		var balloonSpawner = balloonSpawnerObject.GetComponent<BalloonSpawner>();
		balloonSpawner.secondsToGameOver = _secondsToGameOver;
		balloonSpawner.spawnSpeed = _spawnSpeed;
		balloonSpawner.minimumBalloonSize = _minimumBalloonSize;
		balloonSpawner.maximumBalloonSize = _maximumBalloonSize;
		balloonSpawner.minimumBaseBalloonSpeed = _minimumBaseBalloonSpeed;
		balloonSpawner.maximumBaseBalloonSpeed = _maximumBaseBalloonSpeed;
		balloonSpawner.minimumBalloonSizeSpeed = _minimumBalloonSizeSpeed;
		balloonSpawner.maximumBalloonSizeSpeed = _maximumBalloonSizeSpeed;
		
		float startTime = Time.time;
		do {
			_remainingTimeInSeconds = (int) (_secondsToGameOver - (Time.time - startTime));
			_timeToFinishText.text = _remainingTimeInSeconds + " сек";
			yield return null;
		} while (_remainingTimeInSeconds > 0f);
		
		_timeToFinishText.transform.parent.gameObject.SetActive(false);
		Destroy(balloonSpawnerObject);
		StartCoroutine(GameOver());
	}

	IEnumerator GameOver() {
		Balloon.OnClick -= Score;
		
		_h1Text.transform.parent.gameObject.SetActive(true);
		_h1Text.text = "Результат: " + _score;
		_h2Text.text = "Нажмите Пробел для начала игры";
		
		yield return new WaitForSeconds(0.1f);

		StartCoroutine(GameLoop());
	}

	IEnumerator WaitForKeyDown(KeyCode keyCode) {
		while (!Input.GetKeyDown(keyCode)) {
			yield return null;
		}
	}

	private void Score(float size) {
		_score += (int)(_maximumBalloonSize - size) + 1;
		_scoreText.text = "Очки: " + _score;
	}
}
