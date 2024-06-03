import { Counter } from "./components/Counter";
import Login from "./components/Login";

const AppRoutes = [
  {
    index: true,
    element: <Counter />,
  },
  {
    path: "/login",
    element: <Login />,
  },
];

export default AppRoutes;
