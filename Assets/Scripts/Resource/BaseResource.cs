using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLG
{
    /// <summary>
    /// 资源类型枚举
    /// </summary>
    public enum ResourceType
    {
        Currency,
        Aircraft,
        Airport,
        Route,
        Passenger,
        Fuel,
        Staff,
        Maintenance,
        Other
    }

    /// <summary>
    /// 资源基础类 - 所有游戏资源的基类
    /// </summary>
    [Serializable]
    public class BaseResource
    {
        [Header("基础信息")]
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private ResourceType _type;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;

        // 属性访问器
        public string ID => _id;
        public string Name => _name;
        public ResourceType Type => _type;
        public string Description => _description;
        public Sprite Icon => _icon;

        /// <summary>
        /// 初始化资源
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="name">资源名称</param>
        /// <param name="type">资源类型</param>
        /// <param name="description">资源描述</param>
        /// <param name="icon">资源图标</param>
        protected BaseResource(string id, string name, ResourceType type, string description, Sprite icon)
        {
            _id = id;
            _name = name;
            _type = type;
            _description = description;
            _icon = icon;
        }

        /// <summary>
        /// 资源更新逻辑（每帧调用）
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// 资源清理逻辑
        /// </summary>
        public virtual void Cleanup() { }
    }
}