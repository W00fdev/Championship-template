using IndieMarc.Platformer;
using UnityEngine;

namespace IndieMarc.PlatformerDemo
{
    public enum EnemyType { IDLE = 0, SHOOTING, PATROLING };
    public enum ShootingDirection { RIGHT = 0, LEFT };

    /// <summary>
    ///  Скрипт врага (им может быть и ловушка в стене)
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        [Header("Жизни")]
        public float HP;

        [Header("Неуязвимость")]
        public bool Invulnerable;

        [Header("Урон при столкновении со врагом")]
        public float DamageCollision;

        [Header("Тип врага: стоящий / стреляющий / патрулирующий")]
        public EnemyType EnemyType;

        [Space]
        [Header("Префаб пули для врага (стреляющего)")]
        public Projectile EnemyProjectile;

        [Header("Объект-смещение для пули")]
        public Transform OffsetBullet;

        [Header("Скорость стрельбы врага")]
        public float FireRate;

        [Header("Сторона стрельбы: вправо / влево")]
        public ShootingDirection ShootingFace;

        [Space]
        [Header("Две точки-объекта для пути патруля (при нужном типе)")]
        public Transform PatrolingPointA;
        public Transform PatrolingPointB;

        private Rigidbody2D _rb2d;
        private float _fireRateTimer;

        // Свойство определения направления стрельбы
        private Vector3 BulletDirection 
            => (ShootingFace == ShootingDirection.RIGHT) ? Vector3.right : -Vector3.right;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();

            if (EnemyType == EnemyType.SHOOTING && Mathf.Approximately(FireRate, 0))
                throw new System.Exception("Скорость стрельбы врага указана как 0");
        }

        private void Update()
        {
            if (EnemyType == EnemyType.SHOOTING)
            {
                if (_fireRateTimer >= 1f / FireRate)
                {
                    Shoot();
                    _fireRateTimer = 0f;
                }
            }

            _fireRateTimer += Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Бьём игрока при столкновении
            if (TryGetComponent(out PlayerCharacter player))
                player.TakeDamage(DamageCollision);
        }

        public void TakeDamage(float damage)
        {
            if (Invulnerable == true)
                return;

            HP -= damage;

            if (HP <= 0f)
                Destroy(gameObject);
        }

        private void Shoot()
        {
            // Создаём пулю
            var projectileObject = Instantiate(EnemyProjectile, OffsetBullet.position, Quaternion.identity);
            projectileObject.GetComponent<Projectile>().Init(BulletDirection);
        }
    }
}
