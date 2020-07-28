import { Router } from 'express';
import assignmentRouter from '@domains/assignment/infra/http/routes/assignment.routes';

const routes = Router();

routes.use('/assignment', assignmentRouter);

export default routes;
