using Assets.Game.Access.Base;
using Assets.Game.Access.Enums;
using Assets.Game.Core.Collections;

using UnityEngine;

namespace Assets.Game.Access
{
    /// <summary>
    /// Доступ к ресурсам проекта
    /// </summary>
    [CreateAssetMenu(fileName = "ResourcesAccess", menuName ="Access/ResourcesAccess")]
    public class ResourcesAccess: AccessBase<ResourcesAccess>
    {
        #region Fields

        private EnumObjectDictionary<PrefabType, GameObject>
            _prefabs;

        private EnumObjectDictionary<RenderTextureType, CustomRenderTexture>
            _renderTextures;

        private EnumObjectDictionary<SpriteType, Sprite>
            _sprites;

        #endregion

        #region Properties

        /// <summary>
        /// Шейдеры для генерации поля
        /// </summary>
        public ComputeShader CalculateShader;

        /// <summary>
        /// Шейдеры для сортировки ячеек
        /// </summary>
        public ComputeShader SortShader;

        /// <summary>
        /// Префабы
        /// </summary>
        public GameObject[] Prefabs;
        
        /// <summary>
        /// Спрайты
        /// </summary>
        public Sprite[] Sprites;

        /// <summary>
        /// Текстуры для генерации поля
        /// </summary>
        public CustomRenderTexture[] RenderTextures;

        #endregion

        #region Overriden methods

        protected override void OnBegin()
        {
            _prefabs = new EnumObjectDictionary<PrefabType, GameObject>(Prefabs);
            _renderTextures = new EnumObjectDictionary<RenderTextureType, CustomRenderTexture>(RenderTextures);
            _sprites = new EnumObjectDictionary<SpriteType, Sprite>(Sprites);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Инстанцировать префаб
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого компонента</typeparam>
        /// <param name="type">Тип префаба</param>
        /// <returns>Компонент инстанцированного объекта</returns>
        public T CreatePrefab<T>(PrefabType type)
            where T : Component
        {
            return Instantiate(GetPrefab(type)).GetComponent<T>();
        }

        /// <summary>
        /// Возвращает префаб по его типу
        /// </summary>
        /// <param name="type">Тип префаба</param>
        /// <returns>Префаб</returns>
        public GameObject GetPrefab(PrefabType type)
        {
            return _prefabs.GetValue(type);
        }

        /// <summary>
        /// Возвращает текстуру для генерации поля
        /// </summary>
        /// <param name="type">Тип текстуры</param>
        /// <returns>Текстура для генерации</returns>
        public CustomRenderTexture GetRenderTexture(RenderTextureType type)
        {
            return _renderTextures.GetValue(type);
        }

        /// <summary>
        /// Возвращает спрайт по его типу
        /// </summary>
        /// <param name="type">Тип спрайта</param>
        /// <returns>Спрайт</returns>
        public Sprite GetSprite(SpriteType type)
        {
            return _sprites.GetValue(type);
        }

        #endregion
    }
}
