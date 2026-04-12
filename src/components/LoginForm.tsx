import {useState} from "react";
import {useNavigate} from "react-router"
import "../styles/login.css";
import {getErrorMessage} from "../utils/utils.ts";
import Navbar from "./NavBar.tsx";

interface ILoginForm{
    email: string,
    password: string
}

const LoginForm =  () => {
    const navigate = useNavigate();
    
    const [errors, setErrors] = useState({message: ""})
    
    const [formData, setFormdata] = useState<ILoginForm>({
        email: "",
        password: ""
    })
    const ErrorMessageStyle = {
        color: "#B00020",
        backgroundColor: "#FFEBEE",
        borderLeft: "4px solid #D32F2F",
        padding: "8px 12px",
        borderRadius: "4px",
        fontSize: "16px",
        lineHeight: "1.4",
        fontFamily: "Segoe UI, Tahoma, sans-serif",
        marginTop: "6px"
    };
    
    async function Login() {
        try{
            const response = await fetch("https://localhost:7118/api/v1/Auth/login", {
                method: "POST",
                credentials: "include",
                headers:{
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    "email":formData.email,
                    "password": formData.password

                }),
            })
            
            if(!response.ok){
               throw new Error("Invalid credentials provided.")
            }
            else{
                await response.json();
                localStorage.setItem("isLoggedIn", "true")
                navigate("/")
            }
        }
        catch(error){
            const message = getErrorMessage(error)
            setErrors(prevState => ({
                ...prevState,
                message: "Incorrect email or password"
            }))
            console.error(message)
        }
        
        
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        await Login();
    };

    return (
        <>
            <Navbar/>
            <div className="container d-flex justify-content-center align-items-center login-page">

                <div className="card shadow login-card">

                    <div className="login-header">
                        <h3 className="login-title">ExpenseFlow</h3>
                        <small className="login-subtitle">Manage your money smart</small>
                    </div>

                    <div className="card-body p-4">
                        <form onSubmit={handleSubmit}>
                            {errors["message"] && (
                                <p style={ErrorMessageStyle}>{errors["message"]}</p>
                            )}
                            <div className="mb-3">
                                <input
                                    type="email"
                                    title="Your email"
                                    className="form-control login-input"
                                    placeholder="Email address"
                                    minLength={1}
                                    maxLength={100}
                                    name="email"
                                    required
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
                                    title="Your password"
                                    required
                                    className="form-control login-input"
                                    placeholder="Password"
                                    maxLength={50}
                                    minLength={1}
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
        </>
      
    );
}
export default  LoginForm