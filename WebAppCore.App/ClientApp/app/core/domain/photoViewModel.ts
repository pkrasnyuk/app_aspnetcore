export default class PhotoViewModel {
    constructor(
        public id: String,
        public title: String,
        public albumId: String,
        public dateUploaded: Date,
        public fileName: String,
        public fileBytes: String) {
    }
}