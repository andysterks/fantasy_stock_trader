import { Counter } from "./components/Counter";
import Dashboard from "./components/Dashboard";
import Login from "./components/Login";
import Trade from "./components/Trade";
import RegisterUser from "./components/LandingPage/RegisterUser";

const AppRoutes = [
  {
    index: true,
    element: <Login />,
  },
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/register",
    element: <RegisterUser />,
  },
];

export default AppRoutes;
