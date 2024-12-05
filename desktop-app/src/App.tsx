import { UserList } from "./types";
import { invoke } from "@tauri-apps/api/core";
import { useEffect, useState } from "react";

function App() {
   const [userList, setUserList] = useState<UserList>([]);

   async function greet() {
      setUserList(await invoke("list_users"));
   }

   useEffect(() => {
      greet();
   }, []);

   return <>{console.log(userList)}</>;
}

export default App;
