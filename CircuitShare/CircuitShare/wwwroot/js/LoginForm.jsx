class LoginForm extends React.Component {
    render() {
        return (
            <div>
                <img className="imageLogo" src="/img/Share.png" alt="CircuitShare Logo" />
                <div className="login-form-container formAlign">
                <form method="post" action="/Account/LogIn">
                    <div className="form-group row">
                        <div className="col-sm-2"><label>Username:</label></div>
                        <div className="col-sm-4">
                            <input name="Username" className="form-control" required />
                        </div>
                        <div className="col">
                            <span className="text-danger"></span>
                        </div>
                    </div>
                    <div className="form-group row">
                        <div className="col-sm-2"><label>Password:</label></div>
                        <div className="col-sm-4">
                            <input type="password" name="Password" className="form-control" required />
                        </div>
                        <div className="col">
                            <span className="text-danger"></span>
                        </div>
                    </div>
                    <div className="form-group row">
                        <div className="offset-sm-2 col-sm-4">
                            <input type="checkbox" title="Remember Me" name="RememberMe" className="form-check" />
                            <label>Remember Me</label>
                        </div>
                    </div>
                    <div className="row">
                        <div className="offset-2 col-sm-4">
                            <button type="submit" className="btn btn-primary">Log In</button>
                        </div>
                    </div>
                    <div className="row">
                        <div className="offset-2 col-sm-4">
                            Not registered? <a href="/Account/Register">Register as a new user</a>
                        </div>
                    </div>
                    <div className="row">
                        <div className="offset-2 col-sm-4">
                            Forgot Password? <a href="/Account/GetForgotPassword">Reset Password</a>
                        </div>
                    </div>
                </form>
                </div>
            </div>
        );
    }
}

console.log("LoginForm.jsx is loaded");

ReactDOM.render(
    <LoginForm />,
    document.getElementById('react-content-LoginForm')
);
