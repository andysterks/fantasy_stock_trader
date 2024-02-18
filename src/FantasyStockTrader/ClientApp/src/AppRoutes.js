import { Counter } from "./components/Counter";
import Dashboard from "./components/Dashboard";
import { FetchData } from "./components/FetchData";
import Login from "./components/Login";
import Trade from "./components/Trade";

const AppRoutes = [
  {
    index: true,
    element: <Login />,
  },
  {
    path: "/dashboard",
    element: <Dashboard />,
  },
  {
    path: "/trade",
    element: <Trade />,
  },
  {
    path: "/counter",
    element: <Counter />,
  },
  {
    path: "/login",
    element: <Login />,
  },
];

export default AppRoutes;
