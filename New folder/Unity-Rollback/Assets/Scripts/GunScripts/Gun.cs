using UnityEngine;

namespace GunSys
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;

        public void Shoot()
        {
            Bullet bullet = Instantiate(_bullet);

            bullet.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

            bullet.Throw();
        }
    }
}