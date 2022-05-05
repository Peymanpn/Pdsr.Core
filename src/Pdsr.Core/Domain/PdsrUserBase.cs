namespace Pdsr.Core.Domain
{

    /// <summary>
    /// Base user with <typeparamref name="TKey"/> as PK type
    /// </summary>
    /// <typeparam name="TKey">Type of the Id to be used as PK</typeparam>
    public abstract record class PdsrUserBase<TKey> : BaseEntity<TKey>, ISubjectOwnable, IEquatable<ISubjectOwnable>
        where TKey : notnull
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="id">the PK id</param>
        /// <param name="subjectId">user's subject id</param>
        public PdsrUserBase(TKey id, string subjectId) : base(id)
        {
            SubjectId = subjectId;
            Id = id;
        }

        public string SubjectId { get; init; }

        /// <summary>
        /// User creation date
        /// </summary>
        public DateTimeOffset CreatedAtUtc { get; set; }

        /// <summary>
        /// Last time the user has been updated
        /// </summary>
        public DateTimeOffset UpdatedAtUtc { get; set; }

        /// <summary>
        /// Uses SubjectId as a mean to compare two <see cref="PdsrUserBase{TKey}"/> entities.
        /// If <see cref="SubjectId"/> of two entities are equal, then both entities should be equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(
#if NETSTANDARD2_0 || NETSTANDARD2_1 // TODO: use nullable for ISubjectOwnable in case of upgrading to NETSTANDARD2.1
        ISubjectOwnable
#else
        ISubjectOwnable?
#endif
        other)
        {
            if (other is null) return false;
            return SubjectId == other.SubjectId;
        }


    }


    /// <summary>
    /// Base User for unifying user across dependant libs and projects
    /// Default <see cref="PdsrUserBase{TKey}"/> entity with <see cref="string"/> as TKey.
    /// Shorthand for <code>PdsrUserBase{string}</code>
    /// </summary>
    public abstract record PdsrUserBase : PdsrUserBase<string>, ISubjectOwnable, IEquatable<ISubjectOwnable>
    {
        public PdsrUserBase(string subjectId) : base(subjectId, subjectId) { }
        public PdsrUserBase(string id, string subjectId) : base(id, subjectId) { }
    }

    /// <summary>
    /// BaseUser with string as TKey in <see cref="BaseEntity{TKey}"/>
    /// BaseUser is an entity with the same value as Id and SubjectId.
    /// Use <code>builder.Entity(e => e.Id).Ignore();</code> or <code>builder.Entity(e => e.SubjectId).Ignore();</code>
    /// If you want to have only of these fields in the database.
    /// </summary>
    public abstract record class PdsrUserBaseSubjectId : PdsrUserBase<string>, ISubjectOwnable, IEquatable<ISubjectOwnable>, IEquatable<PdsrUserBase>, IEquatable<BaseEntity<string>>
    {
        private string _subjectId;
        /// <summary>
        /// Uses SubjectId as Id of the BaseEntity
        /// </summary>
        /// <param name="subjectId"></param>
        public PdsrUserBaseSubjectId(string subjectId) : base(subjectId, subjectId)
        {
            SubjectId = subjectId;
            _subjectId = subjectId;
        }

        /// <summary>
        /// Hide the base used Id, basically replacing it with SubjectId and use same value for Id and Subject
        /// should Ignore() in EF entity mapping
        /// </summary>
        protected private new string Id { get => _subjectId; set => _subjectId = value; }

        public new string SubjectId { get => _subjectId; init => _subjectId = value; }

        public bool Equals(
#if NETSTANDARD2_0 || NETSTANDARD2_1
        PdsrUserBase
#else
        PdsrUserBase?
#endif
        other)
        {
            if (other is null) return false;
            return SubjectId == other.SubjectId;
        }
    }
}
