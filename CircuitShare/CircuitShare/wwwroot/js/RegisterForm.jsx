class RegisterForm extends React.Component {
    render() {
        return (
            <div>
                <img className="imageLogo" src="/img/Share.png" alt="CircuitShare Logo" />
                <div className="login-form-container formAlign">
                    <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                    <form method="post" asp-controller="Account" asp-action="Register">

                        <div class="form-group row">
                            <div class="col-sm-2"><label>Email:</label></div>
                            <div class="col-sm-4">
                                <input asp-for="Email" class="form-control" />
                            </div>
                            <div class="col">
                                <span asp-validation-for="Email"
                                    class="text-danger"></span>
                            </div>
                        </div>

                        <div class="form-group row">
                            <div class="col-sm-2"><label>Username:</label></div>
                            <div class="col-sm-4">
                                <input asp-for="Username" class="form-control" />
                            </div>
                            <div class="col">
                                <span asp-validation-for="Username"
                                    class="text-danger"></span>
                            </div>
                        </div>

                        <div class="form-group row">
                            <div class="col-sm-2"><label>Password:</label></div>
                            <div class="col-sm-4">
                                <input type="password" asp-for="Password"
                                    class="form-control" />
                            </div>
                            <div class="col">
                                <span asp-validation-for="Password"
                                    class="text-danger"></span>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-2"><label>Confirm Password:</label></div>
                            <div class="col-sm-4">
                                <input type="password" asp-for="ConfirmPassword"
                                    class="form-control" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="offset-2 col-sm-4">
                                <button type="submit" class="btn btn-primary">Register</button>
                            </div>
                        </div>

                        <div class="row">
                            <div class="offset-2 col-sm-4">
                                Already registered? <a asp-action="LogIn">Log In</a>
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
    <RegisterForm />,
    document.getElementById('react-content-RegisterForm')
);




{/*               <!--*/ }
{/*               <input type="hidden" name="RecaptchaResponse" id="RecaptchaResponse" />*/ }
{/*               <div class="g-recaptcha" data-sitekey="6Lcuf2EqAAAAAFs3mQaxffIhnJrF6yltzaOs1EpP"></div>*/ }
{/*-->*/ }



{/*               <!--*/ }
{/*               <script type="text/javascript" src="https://www.google.com/recaptcha/api.js"></script>*/ }
{/*               <script>*/ }
{/*   // Set the recaptcha response when form is submitted*/ }
{/*                   document.querySelector('form').addEventListener('submit', function (event) {*/ }
{/*       var recaptchaResponse = document.getElementById('g-recaptcha-response').value;*/ }
{/*                   document.getElementById('RecaptchaResponse').value = recaptchaResponse;*/ }
{/*   });*/ }
{/*               </script>*/ }
{/*-->*/ }
