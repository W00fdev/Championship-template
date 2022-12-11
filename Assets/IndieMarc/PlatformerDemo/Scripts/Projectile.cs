using IndieMarc.PlatformerDemo;
using UnityEngine;

namespace IndieMarc.Platformer
{
    // Скрипт пули
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("Скорость полета пули")]
        public float Speed;

        [Header("Урон пули")]
        public float Damage;

        [Header("Пуля бьёт по врагам, игроку или всем")]
        public BulletDamageType BulletDamageType;

        public void Init(Vector3 direction)
        {
            // Запускаем пулю в нужном направлении
            GetComponent<Rigidbody2D>().velocity = direction * Speed;

            GetComponent<SpriteRenderer>().flipX = (direction.x > 0) ? false : true;
        }

        // Событие при столкновении пули с кем-либо
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (BulletDamageType == BulletDamageType.PLAYER 
                || BulletDamageType == BulletDamageType.ALL)
            {
                // Если попали в игрока (для вражеской или общей пули)
                if (collision.transform.TryGetComponent(out PlayerCharacter player))
                {
                    player.TakeDamage(Damage);
                }
                else // Если попали во врага (для общей пули)
                if (collision.transform.TryGetComponent(out Enemy enemy) 
                    && BulletDamageType == BulletDamageType.ALL)
                {
                    enemy.TakeDamage(Damage);
                }
                
                DestroyBullet();

            }
            else
            {
                // Пуля стреляет по врагам:
                // BulletDamageType == BulletDamageType.PLAYER

                if (collision.transform.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(Damage);
                    DestroyBullet();
                }
                else // Если попали не в игрока (для пули игрока)
                if (collision.transform.TryGetComponent(out PlayerCharacter _) == false)
                {
                    DestroyBullet();
                }
            }            
        }

        private void DestroyBullet()
        {
            Destroy(gameObject);
        }
    }

}