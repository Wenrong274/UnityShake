using UnityEngine;
using UnityEngine.UI;

namespace hyhy
{
    public class SwitchObj : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private GameObject normal;
        [SerializeField] private GameObject job;

        private void Start()
        {
            Switch(toggle.isOn);
        }

        public void Switch(bool on)
        {
            job.SetActive(on);
            normal.SetActive(!on);
        }
    }
}
