import React from "react";

type ErrorBoundaryProps = {
  children: React.ReactNode;
  onError?: (error: Error, errorInfo: React.ErrorInfo) => void;
};

type ToastErrorBoundaryState = {
  error: Error | null;
};

class ErrorBoundary extends React.Component<ErrorBoundaryProps> {
  constructor(props: ErrorBoundaryProps) {
    super(props);
    this.state = { error: null };
  }

  static getDerivedStateFromError(error: Error): ToastErrorBoundaryState {
    // Store the error but don't replace UI
    return { error };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    this.props.onError?.(error, errorInfo);
  }

  render() {
    return this.props.children;
  }
}

export default ErrorBoundary;