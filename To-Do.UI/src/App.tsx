import { useEffect, useState } from "react";
import {
  addTodo,
  deleteTodo,
  getTodos,
  updateTodo,
} from "./services/todoServices";

interface Todo {
  id: number;
  title: string;
  isCompleted?: boolean;
}

export default function App() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [newTitle, setNewTitle] = useState("");
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editingTitle, setEditingTitle] = useState("");
  const [editingCompleted, setEditingCompleted] = useState(false);

  useEffect(() => {
    fetchTodos();
  }, []);

  const fetchTodos = async () => {
    try {
      const res = await getTodos();
      setTodos(res.data);
    } catch (err) {
      console.error("Error fetching todos:", err);
    }
  };

  const handleAdd = async () => {
    if (!newTitle.trim()) return;

    try {
      const res = await addTodo({ title: newTitle, isCompleted: false });
      setTodos([...todos, res.data]);
      setNewTitle("");
    } catch (err) {
      console.error("Error adding Todos:", err);
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteTodo(id);
      setTodos(todos.filter((t) => t.id !== id));
    } catch (err) {
      console.error("Error deleting todo:", err);
    }
  };

  const startEditing = (
    id: number,
    title: string,
    isCompleted: boolean = false
  ) => {
    setEditingId(id);
    setEditingTitle(title);
    setEditingCompleted(isCompleted);
  };

  const handleUpdate = async (id: number) => {
    if (!editingTitle.trim()) return;

    try {
      const res = await updateTodo(id, {
        title: editingTitle,
        isCompleted: editingCompleted,
      });
      setTodos(todos.map((t) => (t.id === id ? res.data : t)));
      setEditingId(null);
      setEditingTitle("");
    } catch (err) {
      console.error("Error updating todos.", err);
    }
  };

  const toggleCompleted = async (todo: Todo) => {
    try {
      const res = await updateTodo(todo.id, {
        title: todo.title,
        isCompleted: !todo.isCompleted,
      });
      setTodos(todos.map((t) => (t.id === todo.id ? res.data : t)));
    } catch (err) {
      console.error("Error toggling todo:", err);
    }
  };

  return (
    <div style={{ padding: "20px", fontFamily: "Arial" }}>
      <h1>My To-Do list</h1>

      {/* Add todo */}
      <div>
        <input
          type="text"
          placeholder="Enter new todo"
          value={newTitle}
          onChange={(e) => setNewTitle(e.target.value)}
        />
        <button onClick={handleAdd}>Add</button>
      </div>

      {/* List todos */}

      <ul>
        {todos.map((todo) => (
          <li key={todo.id} style={{ marginBottom: "5px" }}>
            <input
              type="checkbox"
              checked={todo.isCompleted || false}
              onChange={() => toggleCompleted(todo)}
            />
            {editingId === todo.id ? (
              <>
                <input
                  type="text"
                  value={editingTitle}
                  onChange={(e) => setEditingTitle(e.target.value)}
                  style={{ marginLeft: "5px" }}
                />

                <button onClick={() => handleUpdate(todo.id)}>Save</button>
                <button onClick={() => setEditingId(null)}>Cancel</button>
              </>
            ) : (
              <>
                <span
                  style={{
                    marginLeft: "5px",
                    textDecoration: todo.isCompleted ? "line-through" : "none",
                  }}
                >
                  {todo.title}
                </span>
                <button
                  onClick={() =>
                    startEditing(todo.id, todo.title, todo.isCompleted || false)
                  }
                  style={{ marginLeft: "10px" }}
                >
                  Edit
                </button>
                <button onClick={() => handleDelete(todo.id)}>Delete</button>
              </>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}
