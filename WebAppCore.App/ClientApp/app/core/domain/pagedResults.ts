export default  class PagedResults<T extends object> {
    constructor(
        public pageNumber: Number,
        public pageSize: Number,
        public totalNumberOfPages: Number,
        public totalNumberOfEntities: Number,
        public entities: T[]) {
    }
}