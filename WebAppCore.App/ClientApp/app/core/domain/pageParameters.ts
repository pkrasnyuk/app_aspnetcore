export default class PageParameters {
    constructor(
        public pageNumber: Number,
        public pageSize: Number,
        public orderBy: String,
        public ascending: Boolean) {
    }
}