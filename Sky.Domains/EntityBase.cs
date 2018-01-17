using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace Sky.Domains
{
    /// <summary>
    /// 领域实体
    /// </summary>
    public abstract class EntityBase<TKey>
    {
        /// <summary>
        /// 初始化领域实体
        /// </summary>
        /// <param name="id">标识</param>
        protected EntityBase(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// 标识
        /// </summary>
        [Required]
        public TKey Id { get; private set; }

        /// <summary>
        /// 相等运算
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Equals(object entity)
        {
            if (entity == null)
            {
                return false;
            }
            if (!(entity is EntityBase<TKey>))
            {
                return false;
            }
            return this==(EntityBase<TKey>)entity;
        }

        /// <summary>
        /// 获取哈希
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// 相等比较
        /// </summary>
        /// <param name="entity1">领域实体1</param>
        /// <param name="entity2">领域实体2</param>
        /// <returns></returns>
        public static bool operator ==(EntityBase<TKey> entity1, EntityBase<TKey> entity2)
        {
            // 两个对象都为空
            if ((object) entity1 == null && (object) entity2 == null)
                return true;

            // 其中一个对象为空
            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            // 对象的主键为空
            if (entity1.Id == null)
                return false;

            // 对象的主键为默认值
            if (entity1.Id.Equals(default(TKey)))
                return false;

            return entity1.Id.Equals(entity2.Id);
        }

        /// <summary>
        /// 不相等比较
        /// </summary>
        /// <param name="entity1">领域实体1</param>
        /// <param name="entity2">领域实体2</param>
        public static bool operator !=(EntityBase<TKey> entity1, EntityBase<TKey> entity2)
        {
            return !(entity1 == entity2);
        }
    }
}
