import "../styles/navbar.css";
import {useNavigate} from "react-router"

const Navbar = () => {
    const navigate = useNavigate();
    
    async function Logout(){
        try{
            await fetch("https://localhost:7118/api/v1/Auth/logout", {
                method: "POST",
                credentials:"include"
            })
            console.log("User has logged out.")
            localStorage.setItem("isLoggedIn", "false")
            navigate("/login")
            
            
        }
        catch(e){
            console.error(e)
        }
    }
    return (
        <nav className="navbar navbar-expand-sm bg-white border-bottom shadow-sm">
            <div className="container py-2">
                {/* Brand */}
                <a className="navbar-brand fw-semibold fs-5" href="/">
                    ExpenseFlow
                </a>

                {/* Toggle */}
                <button
                    className="navbar-toggler border-0"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navbarNav"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>

                {/* Links */}
                <div className="collapse navbar-collapse" id="navbarNav">
                    <ul className="navbar-nav ms-auto gap-2">
                        <li className="nav-item">
                            {
                                localStorage.getItem("isLoggedIn") === "true" &&    
                            <a className="nav-link px-3" href="/">
                                Home
                            </a>
                            }
                        </li>
                        {localStorage.getItem("isLoggedIn") === "true" &&
                            
                        <li className="nav-item">
                            <a className="nav-link px-3" href="/overview">
                                Overview
                            </a>
                        </li>
                        }
                        <li className="nav-item">
                            {
                                localStorage.getItem("isLoggedIn") === "true" ?
                                    <a className="nav-link px-3"  onClick={() => Logout()}>
                                        Logout
                                    </a> :
                                    
                                    <a className="nav-link px-3"  href="/login">
                                        Login
                                    </a>
                                    
                            }
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;