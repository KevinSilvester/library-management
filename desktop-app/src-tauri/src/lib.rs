/// NOTE: The user data would ideally be stored in an encrypted file in the application data dir.
///       The file will a toml file with an array of UserData objects for all the user that have
///       logged in.
///       But for now we are simply going to the values and the api_keys
use serde::{Deserialize, Serialize};
use tauri::State;

/// --- Serializable User Data
#[derive(Debug, Clone, Serialize, Deserialize)]
enum AccessLevel {
    #[serde(rename = "STAFF")]
    Staff,
    #[serde(rename = "MEMBER")]
    Member,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
struct UserData {
    id: u32,
    name: String,
    email: String,
    api_key: String,
    access_level: AccessLevel,
}

type UserDataList = Vec<UserData>;
/// ---

/// --- stub data for application state
fn stub_user_data() -> UserDataList {
    vec![
        UserData {
            id: 1,
            name: "Alice".to_string(),
            email: "alice@email.com".to_string(),
            api_key: "1234".to_string(),
            access_level: AccessLevel::Staff,
        },
        UserData {
            id: 2,
            name: "Bob".to_string(),
            email: "bob@email.com".to_string(),
            api_key: "5678".to_string(),
            access_level: AccessLevel::Member,
        },
    ]
}
/// ---

/// --- Tauri Command that will be compiled to WASM and will be invokable in the frontend
///     typescript code
#[tauri::command]
fn list_users(state: State<'_, UserDataList>) -> Result<Vec<(u32, String)>, String> {
    let users: Vec<(u32, String)> = state
        .inner()
        .iter()
        .map(|u| (u.id, u.name.clone()))
        .collect();
    Ok(users)
}

#[tauri::command]
fn load_user_data(state: State<'_, UserDataList>) -> Result<UserDataList, String> {
    Ok(state.inner().clone())
}
/// ---

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    tauri::Builder::default()
        .manage(stub_user_data())
        .plugin(tauri_plugin_shell::init())
        .invoke_handler(tauri::generate_handler![list_users, load_user_data])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
