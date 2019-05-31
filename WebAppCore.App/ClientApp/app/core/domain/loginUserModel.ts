export default class LoginUserModel {
    constructor(
        public email: String,
        public password: String,
        public rememberMe: Boolean) {
    }
}