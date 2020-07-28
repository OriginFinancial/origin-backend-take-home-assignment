import 'reflect-metadata';
import express, { Request, Response, NextFunction } from 'express';
import cors from 'cors';
import { errors } from 'celebrate';
import 'express-async-errors';
import routes from '@shared/infra/http/routes';
import '@domains/assignment/providers/InsuranceProvider';

interface IErro extends Error {
  statusCode: number;
  type: string;
}

const app = express();

app.use(cors());
app.use(express.json());
app.use(routes);
app.use(errors());

app.use((err: IErro, request: Request, response: Response, _: NextFunction) => {
  if (err.statusCode === 400) {
    return response.status(err.statusCode).json({
      status: 'error',
      message: err.message,
    });
  }

  return response.status(500).json({
    status: 'error',
    message: 'Internal server error',
  });
});

export default app;
