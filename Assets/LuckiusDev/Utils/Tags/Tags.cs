using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LuckiusDev.Utils
{
    [AddComponentMenu("GameObject/Tags")]
    public class Tags : MonoBehaviour
    {
        [SerializeField] private List<Tag> tags;

        private void Awake()
        {
            foreach (Tag tag in tags) gameObject.RegisterGameObjectWithTag(tag);
        }

        private void OnDestroy()
        {
            foreach (Tag tag in tags) gameObject.RemoveGameObjectWithTag(tag);
        }

        public List<Tag> GetTags() => tags;

        public void SetTags(params Tag[] tags) => this.tags = tags.ToList();
    }
}
