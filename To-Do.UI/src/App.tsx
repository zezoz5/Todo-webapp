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
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchTodos();
  }, []);

  const fetchTodos = async () => {
    setLoading(true);
    try {
      const res = await getTodos();
      setTodos(res.data);
      setError(null);
    } catch (err) {
      setError("Error fetching todos:");
    } finally {
      setLoading(false);
    }
  };

  const handleAdd = async () => {
    setLoading(true);
    if (!newTitle.trim()) return;

    try {
      const res = await addTodo({ title: newTitle, isCompleted: false });
      setTodos([...todos, res.data]);
      setNewTitle("");
      setError(null);
    } catch (err) {
      setError("Error adding Todos:");
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    setLoading(true);
    try {
      await deleteTodo(id);
      setTodos(todos.filter((t) => t.id !== id));
      setError(null);
    } catch (err) {
      setError("Error deleting todo:");
    } finally {
      setLoading(false);
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
    setLoading(true);
    if (!editingTitle.trim()) return;

    try {
      const res = await updateTodo(id, {
        title: editingTitle,
        isCompleted: editingCompleted,
      });
      setTodos(todos.map((t) => (t.id === id ? res.data : t)));
      setEditingId(null);
      setEditingTitle("");
      setError(null);
    } catch (err) {
      setError("Error updating todos.");
    } finally {
      setLoading(false);
    }
  };

  const toggleCompleted = async (todo: Todo) => {
    try {
      setLoading(true);
      const res = await updateTodo(todo.id, {
        title: todo.title,
        isCompleted: !todo.isCompleted,
      });
      setTodos(todos.map((t) => (t.id === todo.id ? res.data : t)));
      setError(null);
    } catch (err) {
      setError("Error toggling todo:");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ padding: "20px", fontFamily: "Arial" }}>
      <h1>My To-Do list</h1>

      {error && <p style={{ color: "red" }}>{error}</p>}
      {loading && <p>Loading...</p>}

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
