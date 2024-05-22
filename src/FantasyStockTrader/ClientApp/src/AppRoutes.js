import { Counter } from "./components/Counter";
import Dashboard from "./components/Dashboard";
import { FetchData } from "./components/FetchData";
import Login from "./components/Login";
import Buy from "./components/Buy";
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
    path: "/trade/buy/:symbol",
    element: <Buy />,
  },
  {
    path: "/counter",
    element: <Counter />,
  },
  {
    path: "/fetch-data",
    element: <FetchData />,
  },
];

export default AppRoutes;
