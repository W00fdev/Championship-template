using IndieMarc.PlatformerDemo;
using UnityEngine;

namespace IndieMarc.Platformer
{
    // Скрипт стрельбы персонажа
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerShooting : MonoBehaviour
    {
        [Header("Префаб пули")]
        public Projectile ProjectilePrefab;

        [Header("Объект-смещение для пули")]
        public Transform OffsetBullet;

        [Header("Скорострельность в сек.")]
        public int FireRate;

        private PlayerControls _playerControls;
        private PlayerCharacter _playerCharacter;

        private float _fireRateTimer;

        private void Start()
        {
            _playerControls = PlayerControls.Instance;
            _playerCharacter = GetComponent<PlayerCharacter>();

            if (Mathf.Approximately(FireRate, 0))
                throw new System.Exception("Скорость стрельбы героя указана как 0");
        }

        private void Update()
        {
            // При нажатии на стрельбу с указанным темпом стрельбы
            if (_playerControls.GetFireDown() 
                && _fireRateTimer >= (1f / FireRate))
            {
                // Стреляем и обнуляем таймер
                Shoot();
                _fireRateTimer = 0f;
            }

            _fireRateTimer += Time.deltaTime;
        }

        private void Shoot()
        {
            // Создаём пулю
            var projectileObject = Instantiate(ProjectilePrefab, OffsetBullet.position, Quaternion.identity);
            projectileObject.GetComponent<Projectile>().Init(_playerCharacter.FaceDirection);
        }
    }

}