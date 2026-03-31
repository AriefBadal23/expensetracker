import {useState} from "react";
import {useNavigate} from "react-router"
import "./login.css";
interface ILoginForm{
    email: string,
    password: string
}

const LoginForm =  () => {
    const navigate = useNavigate();
    
    const [formData, setFormdata] = useState<ILoginForm>({
        email: "",
        password: ""
    })
    console.log(formData)
    
    async function Login() {
        try{
            const response = await fetch("https://localhost:7118/api/v1/Auth/login", {
                method: "POST",
                credentials: "include",
                headers:{
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    "email":"arief@test.nl",
                    "password": "Marvel01234@!"

                }),
            })
            if(!response.ok){
                return;
            }
            const result = await response.json();
            console.log(result)
            navigate("/")
        }
        catch(e:unknown){
            console.log(e)
        }
        
        
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        await Login();
    };

    return (
        <div className="container d-flex justify-content-center align-items-center login-page">

            <div className="card shadow login-card">

                <div className="login-header">
                    <h3 className="login-title">ExpenseFlow</h3>
                    <small className="login-subtitle">Manage your money smart</small>
                </div>

                <div className="card-body p-4">
                    <form onSubmit={handleSubmit}>

                        <div className="mb-3">
                            <input
                                type="email"
                                className="form-control login-input"
                                placeholder="Email address"
                                name="email"
                                value={formData.email}
                                onChange={(e) =>
                                    setFormdata(prev => ({
                                        ...prev,
                                        email: e.target.value
                                    }))
                                }
                            />
                        </div>

                        <div className="mb-4">
                            <input
                                type="password"
                                className="form-control login-input"
                                placeholder="Password"
                                name="password"
                                value={formData.password}
                                onChange={(e) =>
                                    setFormdata(prev => ({
                                        ...prev,
                                        password: e.target.value
                                    }))
                                }
                            />
                        </div>

                        <button type="submit" className="btn w-100 login-button">
                            Login
                        </button>

                    </form>
                </div>
            </div>
        </div>
    );
}
export default  LoginForm