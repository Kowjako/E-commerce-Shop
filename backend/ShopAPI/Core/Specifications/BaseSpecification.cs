﻿using System.Linq.Expressions;

namespace Core.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        public BaseSpecification() { }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }  

        protected void AddInclude(Expression<Func<T, object>> include)
        {
            Includes.Add(include);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderBy) 
            => OrderBy = orderBy;

        protected void AddOrderByDesc(Expression<Func<T, object>> orderByDesc) 
            => OrderByDescending = orderByDesc;

        protected void ApplyPaging(int skip, int take) =>
            (Skip, Take, IsPagingEnabled) = (skip, take, true);
    }
}
