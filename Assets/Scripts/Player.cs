using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Player : MonoBehaviour {

    AxisChange fireAxis;

    static string horizontalAxis = "Horizontal";
    static string verticalAxis = "Vertical";

    public float Speed = 1f;

    public GameObject shotPrefab;
    [SerializeField] private  GameObject fishOptionPrefab;

    [SerializeField] private GameObject mainCameraObj;
    private Camera mainCamera;

    [SerializeField] private ShotType shotMode;

    List<FishOption> fishOptions;

	[SerializeField] GameObject particleObject;
    ParticleSystem particle;

    [SerializeField] private GameObject soundEffectObj;
    private SoundEffect soundEffect;

    enum ShotType {
        Normal,
        Twin,
        Triple,
    }

    void Start () {
		particle = particleObject.GetComponent<ParticleSystem>();
        soundEffect = soundEffectObj.GetComponent<SoundEffect>();

        fireAxis = new AxisChange("Fire1");
        mainCamera = mainCameraObj.GetComponent<Camera>();
        fishOptions = new List<FishOption>();

        createOption();
    }

    void Update () {
        // プレイヤーの移動
        float delta = Time.deltaTime;
        var h = Input.GetAxis(horizontalAxis);
        var v = Input.GetAxis(verticalAxis);

        var old = transform.localPosition;
        var newPosition = new Vector3(
            old.x + h * delta * Speed,
            old.y + v * delta * Speed,
            old.z);

        // 画面端に出ないようにする
        var topLeft = mainCamera.ViewportToWorldPoint(new Vector2(0,0));
        var bottomRight = mainCamera.ViewportToWorldPoint(new Vector2(1,1));
        newPosition.x = Mathf.Clamp(newPosition.x, topLeft.x, bottomRight.x);
        newPosition.y = Mathf.Clamp(newPosition.y, topLeft.y, bottomRight.y);
        
        transform.localPosition = newPosition;

        // shot入力チェック
        fireAxis.Update();
        if (fireAxis.ChangePositive) {
            shot();
        }
    }

    void shot() {
        var c = GetComponent<Animator>();
        c.SetTrigger("shot-trig");
        c.SetBool("shot", true);
        var shotPosition = transform.localPosition;
        shotPosition.x += 0.8f;
        switch (shotMode) {
            case ShotType.Normal:
                Instantiate(shotPrefab, shotPosition, Quaternion.identity);
                break;
            case ShotType.Twin:
                var shotPosition1 = shotPosition;
                var shotPosition2 = shotPosition;
                shotPosition1.y += 0.2f;
                shotPosition2.y += -0.2f;
                Instantiate(shotPrefab, shotPosition1, Quaternion.identity);
                Instantiate(shotPrefab, shotPosition2, Quaternion.identity);
                break;
            case ShotType.Triple:
                var o1 = Instantiate (shotPrefab, shotPosition, Quaternion.identity);
                var r1 = Quaternion.Euler(0, 0, 30);
                o1.GetComponent<Shot>().deltaVector = r1 * o1.GetComponent<Shot>().deltaVector;

                Instantiate(shotPrefab, shotPosition, Quaternion.identity);

                var o2 = Instantiate (shotPrefab, shotPosition, Quaternion.identity);
                var r2 = Quaternion.Euler(0, 0, -30);
                o2.GetComponent<Shot>().deltaVector = r2 * o2.GetComponent<Shot>().deltaVector;

                break;
        }

        fishOptions.ForEach(o => {
            o.shot();
        });
        soundEffect.playShot();
    }

    void PowerUp(PowerBlock po) {
        PowerBlock.PoweUpType type = po.Type;

        switch (type) {
            case PowerBlock.PoweUpType.Plain:
                soundEffect.playPowerUp1();
                GameSystemData.Instance.Score.Value += 100;
                break;
            case PowerBlock.PoweUpType.SpeedUp:
                Speed += 0.5f;
                soundEffect.playPowerUp2();
                break;
            case PowerBlock.PoweUpType.TwinShot:
                shotMode = ShotType.Twin;
                soundEffect.playPowerUp2();
                break;
            case PowerBlock.PoweUpType.TripleShot:
                shotMode = ShotType.Triple;
                soundEffect.playPowerUp2();
                break;
            case PowerBlock.PoweUpType.Option:
                createOption();
                soundEffect.playPowerUp2();
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "PowerUp") {
            PowerUp(other.GetComponent<PowerBlock>());
            Destroy(other.gameObject);
        }
    }

    void createOption() {
        var o = Instantiate(fishOptionPrefab, transform.localPosition, Quaternion.identity);
        var fishOption = o.GetComponent<FishOption>();
        fishOptions.Add(fishOption);
        fishOption.PlayerObj = this;
        fishOption.positionNum = fishOptions.Count * 15;
    }

    public void OnDamage() {
        particle.Emit(100);
        particleObject.transform.localPosition = transform.localPosition;

        // Playerを参照している箇所があるので、Destroyしてはいけない
        this.gameObject.SetActive(false);

        fishOptions.ForEach(opt => {
            opt.enabled = false;
        });

        var context = SynchronizationContext.Current;

        Task.Run(async() => {
            await Task.Delay(2000);
            context.Post((state) => {
                SceneManager.LoadScene("GameOverScene");
            }, null);
        });
    }
}
