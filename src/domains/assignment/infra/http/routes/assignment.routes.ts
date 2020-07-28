import { Router } from 'express';
import requestValidation from '../middlewares/requestValidation';
import CreateAssignmentController from '../controllers/CreateAssignmentController';

const assignmentRouter = Router();
const createAssignmentController = new CreateAssignmentController();

assignmentRouter.use(requestValidation);
assignmentRouter.post('/', createAssignmentController.create);

export default assignmentRouter;
